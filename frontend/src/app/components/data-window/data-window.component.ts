import { Component, OnInit } from '@angular/core';
import { VisualizedDataComponent } from "./visualized-data/visualized-data.component";
import { TextOutputComponent } from "./text-output/text-output.component";
import { DatasetModel } from "../../models/dataset.model";
import { DatasetsService } from "../../services/datasets.service";

@Component({
    selector: 'app-data-window',
    imports: [
        VisualizedDataComponent,
        TextOutputComponent
    ],
    templateUrl: './data-window.component.html',
    styleUrl: './data-window.component.scss'
})
export class DataWindowComponent implements OnInit {
    dataset: DatasetModel | undefined;

    constructor(private datasetsService: DatasetsService) {
    }

    ngOnInit() {
        this.datasetsService.selectedDataset$.subscribe((dataset) => {
            this.dataset = dataset;
        });
    }
}
