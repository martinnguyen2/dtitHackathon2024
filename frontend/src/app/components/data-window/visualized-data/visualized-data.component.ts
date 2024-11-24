import { ChangeDetectionStrategy, Component, Input, ViewChild } from '@angular/core';
import {ChartData, ChartOptions} from 'chart.js';
import {ChartjsComponent, ChartjsModule} from '@ctrl/ngx-chartjs';
import { ChatQueryResponseModel } from '../../../models/chat-query-response.model';
import { GraphData } from '../../../models/graph-data.model';
import { SpinnerComponent } from "../../spinner/spinner.component";



@Component({
  selector: 'app-visualized-data',
  imports: [ChartjsModule, ChartjsModule],
  templateUrl: './visualized-data.component.html',
  styleUrl: './visualized-data.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class VisualizedDataComponent {
  @ViewChild('graphId', { static: true }) graphElement!: ChartjsComponent;
  @Input() set graphData(data: ChatQueryResponseModel | undefined){
    if (data?.type == 'visualize'){
      this.values = data?.graphData?.map(item => +item.value);
      this.labels = data?.graphData?.map(item => item.label);
      this.chartType = data?.chartType;
      this.xLabel = data?.xLabel;
      this.yLabel = data?.yLabel;
    }
  };

  labels: string[] | undefined = [];
  values: number[] | undefined = [];
  chartType: string | undefined = '';
  xLabel: string | undefined = '';
  yLabel: string | undefined = '';
  private _data: GraphData[] = [];

  get graphData(): GraphData[]{
    return this._data;
  }

  options: ChartOptions = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
      },
      title: {
        display: true,
        text: 'Chart.js Doughnut Chart',
      },
    },
  };

  constructor(){}

  get dataState(): ChartData{
    return {
      labels: this.labels,
      datasets: [
        {
          label: this.xLabel,
          data: this.values!,
          backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
        },
      ],
    };
  }

}
