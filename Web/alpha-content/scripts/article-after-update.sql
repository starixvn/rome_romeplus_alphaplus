USE alpha_centraldoc;
DROP TRIGGER article_after_update;

USE alpha_centraldoc;
DELIMITER $$
CREATE TRIGGER article_after_update
AFTER UPDATE ON articles
FOR EACH ROW
BEGIN
  IF (old.published_at IS NOT NULL AND new.published_at IS NULL) THEN
	INSERT INTO admin_actions (actor_id, created_at, action, target_collection, target_info, target_value)
	VALUES (new.updated_by_id, CURRENT_TIMESTAMP(), 'UNPUBLISHED', 'ARTICLE',  old.name, JSON_OBJECT(
        'id', old.id, 
        'title', old.title, 
        'created_at', old.created_at, 
        'updated_at', old.updated_at, 
        'published_at', old.published_at, 
        'created_by_id', old.created_by_id, 
        'updated_by_id', old.updated_by_id, 
        'locale', old.locale, 
        'slug', old.slug, 
        'summary', old.summary,
        'content', old.content, 
        'name', old.name, 
        'vuid', old.vuid, 
        'version_number', old.version_number, 
        'version_comment', old.version_comment,
        'is_visible_in_list_view', old.is_visible_in_list_view)); 
  END IF;
  
  IF (old.published_at IS NULL AND new.published_at IS NOT NULL) THEN
	INSERT INTO admin_actions (actor_id, created_at, action, target_collection, target_info, target_value)
	VALUES (new.updated_by_id, CURRENT_TIMESTAMP(), 'PUBLISHED', 'ARTICLE',  old.name, JSON_OBJECT(
        'id', old.id, 
        'title', old.title, 
        'created_at', old.created_at, 
        'updated_at', old.updated_at, 
        'published_at', old.published_at, 
        'created_by_id', old.created_by_id, 
        'updated_by_id', old.updated_by_id, 
        'locale', old.locale, 
        'slug', old.slug, 
        'summary', old.summary,
        'content', old.content, 
        'name', old.name, 
        'vuid', old.vuid, 
        'version_number', old.version_number, 
        'version_comment', old.version_comment,
        'is_visible_in_list_view', old.is_visible_in_list_view)); 
  END IF;
  
  SET @max_revision = IFNULL((SELECT MAX(revision) FROM article_drafts WHERE vuid = old.vuid), 0);
  
  IF NOT EXISTS (SELECT 1 FROM article_drafts WHERE vuid = old.vuid AND version_number = old.version_number) THEN
	  INSERT INTO article_drafts (revision, created_at, title, summary, name, slug, content, vuid, version_number)
	  VALUES (@max_revision + 1, CURRENT_TIMESTAMP(), old.title, old.summary, old.name, old.slug, old.content, old.vuid, old.version_number); 
  END IF;
  
   IF (@max_revision >= 10) THEN
	  UPDATE article_drafts SET revision = revision - 1 WHERE vuid = old.vuid;
	  DELETE FROM article_drafts WHERE revision = 0;
  END IF;
END$$   
DELIMITER ; 


