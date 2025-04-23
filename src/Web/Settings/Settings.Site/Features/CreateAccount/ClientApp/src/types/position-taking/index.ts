import { SubMessagesResult } from '../';

export interface PTTemplateSetting {
  groupId: number;
  subGroupId: number;
  pt: number;
}

export interface PTTemplate {
  id: number;
  name: string;
  isDefault: boolean;
  settings: PTTemplateSetting[];
  isValid?: boolean;
}

export interface CreatePTTemplateResult extends SubMessagesResult {
  templateId: number;
}
