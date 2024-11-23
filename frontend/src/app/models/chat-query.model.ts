import { DatasetModel } from './dataset.model';

export interface ChatQueryModel {
  prompt: string;
  dataset: DatasetModel;
  cacheId?: string;
}
