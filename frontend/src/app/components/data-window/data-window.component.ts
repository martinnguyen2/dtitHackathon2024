import { Component, OnInit } from '@angular/core';
import { VisualizedDataComponent } from "./visualized-data/visualized-data.component";
import { TextOutputComponent } from "./text-output/text-output.component";
import { DatasetService } from "../../services/dataset.service";
import { DatasetModel } from "../../models/dataset.model";

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

    constructor(private datasetService: DatasetService) {
    }

    ngOnInit() {
        this.datasetService.selectedDataset$.subscribe((dataset) => {
            this.dataset = dataset;
        });
    }
}
