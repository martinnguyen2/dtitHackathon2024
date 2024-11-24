import { Component, OnInit } from '@angular/core';
import { DatasetModel } from '../../models/dataset.model';
import { DatasetsService } from '../../services/datasets.service';
import { ToastrService } from 'ngx-toastr';
import { MatIcon } from '@angular/material/icon';
import { ChatQueryService } from '../../services/chat-query.service';

@Component({
  selector: 'app-tool-bar',
  imports: [
    MatIcon
  ],
  templateUrl: './tool-bar.component.html',
  styleUrl: './tool-bar.component.scss'
})
export class ToolBarComponent implements OnInit {
  datasets: DatasetModel[] = [];
  selectedDataset: DatasetModel | undefined;
  predictedModel: number = 0;

  constructor(private datasetsService: DatasetsService, private toastService: ToastrService, private chatQuery: ChatQueryService) {
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

  setPredictiveSet(input: number) {
    this.predictedModel = input;
    this.datasetsService.setPredictiveSet(input);
  }

  onFileSelected(event: any) {

    const file: File = event.target.files[0];
    if (!file)
      return;

    const dataToSend = new FormData()

    dataToSend.append("file", file)

    this.datasetsService.postDataset(dataToSend).subscribe(
      response => this.toastService.success(response.message, "Success:", {positionClass: "toast-bottom-right"}),
      error => this.toastService.error(error.error.message, "Error:", {positionClass: "toast-bottom-right"})
    );
  }

  reset() {
    this.datasetsService.setDataset(undefined);
    this.datasetsService.setPredictiveSet(0);
    this.chatQuery.prompt = '';
    this.selectedDataset = undefined;
    this.predictedModel = 0;
  }
}
