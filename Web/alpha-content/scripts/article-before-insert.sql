USE alpha_centraldoc;
DROP TRIGGER IF EXISTS article_before_insert;

USE alpha_centraldoc;
DELIMITER $$
CREATE TRIGGER article_before_insert
BEFORE INSERT ON articles
FOR EACH ROW
BEGIN
  IF (new.slug IS NULL) THEN
	  SET new.slug = REPLACE(LOWER(new.title), ' ', '-');
	  SET new.slug = REPLACE(LOWER(new.slug), '/', '-');
  END IF;

  IF (new.info_in_english IS NULL) THEN
	  SET new.info_in_english = CONCAT("[", new.locale, "] ", new.name);
  END IF;
  
  IF EXISTS (SELECT id FROM articles WHERE vuid=new.vuid AND locale=new.locale AND version_number=new.version_number) THEN
    SET @orgVuid = SUBSTRING(new.vuid, 1, 36);
     
    IF EXISTS (SELECT vuid FROM articles WHERE LENGTH(vuid) > 37) THEN
		  SET @extendVuid = (SELECT MAX(CAST(SUBSTRING(vuid, 38, 12) AS UNSIGNED)) FROM articles WHERE LENGTH(vuid) > 37) ;
    ELSE
		  SET @extendVuid = 0;
    END IF;
     
    SET @newExtendVuid = @extendVuid + 1;
    SET new.vuid = CONCAT(@orgVuid, '-', LPAD(@newExtendVuid, 12, '0'));
  END IF;

  SET new.published_count = 0;
END$$   
DELIMITER ; 