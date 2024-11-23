import { Component, OnInit } from '@angular/core';
import { DatasetService } from '../../services/dataset.service';
import { DatasetModel } from '../../models/dataset.model';

@Component({
  selector: 'app-tool-bar',
  imports: [],
  templateUrl: './tool-bar.component.html',
  styleUrl: './tool-bar.component.scss'
})
export class ToolBarComponent implements OnInit {
  datasets: DatasetModel[] = [];

  constructor(private datasetService: DatasetService) {
  }

  ngOnInit() {
    this.getDatasets();
  }

  getDatasets() {
    this.datasetService.getDatasets().subscribe((datasets) => {
      this.datasets = datasets;
    });
  }

}
