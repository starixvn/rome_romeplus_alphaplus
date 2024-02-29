USE alpha_centraldoc;
DROP TRIGGER IF EXISTS article_before_update;

USE alpha_centraldoc;
DELIMITER $$
CREATE TRIGGER article_before_update
BEFORE UPDATE ON articles
FOR EACH ROW
BEGIN
  IF (new.slug is null) THEN
	  SET new.slug = REPLACE(LOWER(new.title), ' ', '-');
	  SET new.slug = REPLACE(LOWER(new.slug), '/', '-');
  END IF;
  IF (new.info_in_english is null) THEN
	  SET new.info_in_english = CONCAT("[", new.locale, "] ", new.name);
  END IF;
  IF (old.published_at is not null AND new.published_at is null) THEN
    INSERT INTO articles_environment_links (article_id, environment_id)
    VALUES (new.id, (SELECT  id FROM environments WHERE name='PROD'))
    ON DUPLICATE KEY UPDATE article_id=new.id; 
  END IF;
END$$   
DELIMITER ; 
