import { create } from 'zustand';
import { PTTemplateSlide, createPTTemplateSlice } from './ptTemplateSlide';
import { PTSettingsSlide, createPTSettingsSlice } from './ptSettingsSlide';

const usePTTemplateStore = create<PTTemplateSlide & PTSettingsSlide>()((...a) => ({
  ...createPTTemplateSlice(...a),
  ...createPTSettingsSlice(...a),
}));

export default usePTTemplateStore;
