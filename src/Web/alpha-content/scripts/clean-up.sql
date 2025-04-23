USE alpha_centraldoc;
DELETE FROM categories_articles_links WHERE article_id IS NULL

USE alpha_centraldoc;
DROP TRIGGER IF EXISTS categories_articles_links_before_insert;