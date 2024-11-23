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

  ngOnInit() {
    this.getDatasets();
  }

  getDatasets() {
    this.datasetService.getDatasets().subscribe((datasets) => {
      this.datasets = datasets;
    });
  }

  constructor(private datasetService: DatasetsService){}

  public onFileSelected(event : any){

    const file : File = event.target.files[0];
    if(!file)
      return;

    const dataToSend = new FormData()

    dataToSend.append("file",file)

    this.datasetService.postDataset(dataToSend).subscribe(
      next => console.log(next)
    );
  }
}
