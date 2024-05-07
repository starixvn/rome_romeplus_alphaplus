import matter from 'gray-matter';
import { remark } from 'remark';
import html from 'remark-html';

export async function markdownToHTML(content: string) {
  // Use remark to convert markdown into HTML string
  const matterResult = matter(content);
  const processedContent = await remark().use(html).process(matterResult.content)
  return processedContent.toString();
}
