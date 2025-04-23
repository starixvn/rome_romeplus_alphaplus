USE alpha_centraldoc;
DROP PROCEDURE IF EXISTS FixCategoryArticleLink;

DELIMITER $$
CREATE PROCEDURE FixCategoryArticleLink(
	caid int,
    cid int,
    aid int)
BEGIN
	UPDATE categories_articles_links
	SET article_id = (
		SELECT id FROM (
			SELECT a2.id, 1 AS aorder FROM articles a1, articles a2, categories c
			WHERE a1.id = aid AND a2.vuid = a1.vuid AND c.id = cid AND a2.locale = c.locale AND a2.published_at IS NOT NULL 
			UNION
			SELECT a2.id, 2 AS aorder FROM articles a1, articles a2, categories c
			WHERE a1.id = aid AND a2.vuid = a1.vuid AND c.id = cid AND a2.locale = c.locale AND a2.published_count = 1 AND NOT EXISTS (
				SELECT a3.id FROM articles a3 WHERE a3.vuid = a2.vuid AND a3.locale = a2.locale AND a3.version_number > a2.version_number
			)
			UNION
			SELECT a2.id, 3 AS aorder FROM articles a1, articles a2, categories c
			WHERE a1.id = aid AND a2.vuid = a1.vuid AND c.id = cid AND a2.locale = c.locale AND NOT EXISTS (
				SELECT a3.id FROM articles a3 WHERE a3.vuid = a2.vuid AND a3.locale = a2.locale AND a3.version_number > a2.version_number
			)
		) AS ids
		ORDER BY aorder ASC
		LIMIT 1
	)
	WHERE id=caid;
END;
