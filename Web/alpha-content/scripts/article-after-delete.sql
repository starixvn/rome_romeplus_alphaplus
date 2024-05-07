USE alpha_centraldoc;
DROP TRIGGER IF EXISTS article_after_delete


USE alpha_centraldoc;
DELIMITER $$
CREATE TRIGGER article_after_delete
AFTER DELETE ON articles
FOR EACH ROW
BEGIN
  INSERT INTO admin_actions (actor_id, created_at, action, target_collection, target_info, target_value)
  VALUES (0, CURRENT_TIMESTAMP(), 'DELETE', 'ARTICLE',  old.name, JSON_OBJECT(
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
END$$   
DELIMITER ; 
