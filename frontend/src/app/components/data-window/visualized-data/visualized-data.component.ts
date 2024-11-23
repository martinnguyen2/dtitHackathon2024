import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import {ChartData, ChartOptions} from 'chart.js';
import {ChartjsComponent, ChartjsModule} from '@ctrl/ngx-chartjs';
import { DatasetsService } from '../../../services/datasets.service.js';
import { ChatQueryService } from '../../../services/chat-query.service.js';


@Component({
  selector: 'app-visualized-data',
  imports: [ChartjsModule, ChartjsModule],
  templateUrl: './visualized-data.component.html',
  styleUrl: './visualized-data.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class VisualizedDataComponent implements OnInit{
  @ViewChild('graphId', { static: true }) graphElement!: ChartjsComponent;
  @Input() set graphData(data: GraphData[]){
    this.values = data.map(item => +item.value);
    this.labels = data.map(item => item.label);
  };

  labels: string[] = [];
  values: number[] = [];
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

  constructor(private datasetService : DatasetsService, private chatQueryService: ChatQueryService){}

  ngOnInit() {
    this.datasetService.selectedDataset$.subscribe(item => this.chatQueryService.getGraph(item.name).subscribe(
      res => console.log(res)
   ))
  }

  get dataState(): ChartData{
    return {
      labels: this.labels,
      datasets: [
        {
          label: 'Dataset 1',
          data: this.values,
          backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
        },
      ],
    };
  }

}
