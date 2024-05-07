USE alpha_centraldoc;
DROP TRIGGER IF EXISTS categories_articles_links_before_insert;

USE alpha_centraldoc;
DELIMITER $$
CREATE TRIGGER categories_articles_links_before_insert
BEFORE INSERT ON categories_articles_links
FOR EACH ROW
BEGIN
  IF NOT EXISTS (SELECT a.id FROM categories c, articles a WHERE c.id = new.category_id AND a.id = new.article_id AND c.locale = a.locale) THEN
	SET new.article_id = (
		SELECT a2.id FROM articles a1, articles a2, categories c
        WHERE a1.id = new.article_id AND a2.vuid = a1.vuid AND a2.published_at IS NOT NULL AND c.id = new.category_id AND a2.locale = c.locale
    );
  END IF;
END$$   
DELIMITER ; 