import { StateCreator } from 'zustand';

import { PTTemplate } from '@/types/position-taking';

export type PTTemplateSlide = {
  editingTemplateId: number;
  setEditingTemplateId: (id: number) => void;
  templates: PTTemplate[];
  updateTemplates: (data: PTTemplate[]) => void;
  addTemplate: (template: PTTemplate) => void;
};

export const createPTTemplateSlice: StateCreator<PTTemplateSlide, [], [], PTTemplateSlide> = (set) => ({
  editingTemplateId: 0,
  setEditingTemplateId: (id) => set(() => ({ editingTemplateId: id })),
  templates: [],
  updateTemplates: (data) => set(() => ({ templates: data })),
  addTemplate: (template) =>
    set((state) => ({
      templates: [
        template,
        ...state.templates.map((item) => (template.isDefault ? { ...item, isDefault: false } : item)),
      ],
    })),
});
