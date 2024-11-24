import { Component, OnInit } from '@angular/core';
import { DatasetModel } from '../../models/dataset.model';
import { DatasetsService } from '../../services/datasets.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-tool-bar',
  imports: [],
  templateUrl: './tool-bar.component.html',
  styleUrl: './tool-bar.component.scss'
})
export class ToolBarComponent implements OnInit {
  datasets: DatasetModel[] = [];
  selectedDataset: DatasetModel | undefined;
  predictedModel:number = 0;

  constructor(private datasetsService: DatasetsService, private toastService: ToastrService) {
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

  setPredictiveSet(input: number){
    this.predictedModel = input;
    this.datasetsService.setPredictiveSet(input);

    if(input === 1)
      this.toastService.warning("SVM is experimental function","⚠⚠⚠⚠⚠",{positionClass: "toast-bottom-right"});
  }

  onFileSelected(event : any){

    const file : File = event.target.files[0];
    if(!file)
      return;

    const dataToSend = new FormData()

    dataToSend.append("file", file)

    this.datasetsService.postDataset(dataToSend).subscribe(
      response => this.toastService.success(response.message,"Success:",{positionClass: "toast-bottom-right"}),
      error =>  this.toastService.error(error.error.message,"Error:",{positionClass: "toast-bottom-right"})
    );
  }
}
