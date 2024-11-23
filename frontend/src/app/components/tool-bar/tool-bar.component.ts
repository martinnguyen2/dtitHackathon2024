import { Component, OnInit } from '@angular/core';
import { DatasetModel } from '../../models/dataset.model';
import { DatasetsService } from '../../services/datasets.service';

@Component({
  selector: 'app-tool-bar',
  imports: [],
  templateUrl: './tool-bar.component.html',
  styleUrl: './tool-bar.component.scss'
})
export class ToolBarComponent implements OnInit {
  datasets: DatasetModel[] = [];
  selectedDataset: DatasetModel | undefined;

  constructor(private datasetsService: DatasetsService) {
  }

  ngOnInit() {
    this.getDatasets();
  }

  getDatasets() {
    this.datasetsService.getDatasets().subscribe((datasets) => {
      this.datasets = datasets;
    });
  }

  setDataset(dataset: DatasetModel) {
    this.datasetsService.setDataset(dataset);
    this.selectedDataset = dataset;
  }

  onFileSelected(event : any){

    const file : File = event.target.files[0];
    if(!file)
      return;

    const dataToSend = new FormData()

    dataToSend.append("file",file)

    this.datasetService.postDataset(dataToSend).subscribe(
      response => console.log(response),
      error =>  console.log(error.error.message)
    );
  }
}
