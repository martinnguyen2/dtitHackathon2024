import { GraphData } from './graph-data.model';

export interface ChatQueryResponseModel {
  type: string;
  chartType?: string;
  textOutput?: string;
  graphData?: GraphData[];
  xLabel?: string;
  yLabel?: string;
  cacheId: string;
  dataset?: string;
}
