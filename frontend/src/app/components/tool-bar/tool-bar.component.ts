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
  selectedDataset: DatasetModel | undefined;

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

  setDataset(dataset: DatasetModel) {
    this.datasetService.setDataset(dataset);
    this.selectedDataset = dataset;
  }

  onFileSelected(event : any){

    const file:File = event.target.files[0];
    if(!file)
      return;

    const dataToSend = new FormData()

    dataToSend.append("file",file)

    this.datasetService.postDataset(dataToSend).subscribe(
      next => console.log(next)
    );
  }
}
