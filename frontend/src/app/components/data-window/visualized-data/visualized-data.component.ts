import {Component, OnInit, ViewChild} from '@angular/core';
import {ChartData, ChartOptions} from 'chart.js';
import {ChartjsComponent, ChartjsModule} from '@ctrl/ngx-chartjs';
import { DatasetsService } from '../../../services/datasets.service.js';
import { ChatQueryService } from '../../../services/chat-query.service.js';
import { GraphData } from '../../../models/graph-data.model.js';


@Component({
  selector: 'app-visualized-data',
  imports: [ChartjsModule, ChartjsModule],
  templateUrl: './visualized-data.component.html',
  styleUrl: './visualized-data.component.scss'
})
export class VisualizedDataComponent implements OnInit{
  @ViewChild('graphId', { static: true }) graphElement!: ChartjsComponent;
  data: ChartData = this.getDataState();
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

  

  getDataState(): ChartData{
    return {
      labels: ["green","red","Blue"],
      datasets: [
        {
          label: 'Dataset 1',
          data: [100,200,300],
          backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
        },
      ],
    };
  }
}
