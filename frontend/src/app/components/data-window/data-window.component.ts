import { Component, OnInit } from '@angular/core';
import { VisualizedDataComponent } from "./visualized-data/visualized-data.component";
import { TextOutputComponent } from "./text-output/text-output.component";
import { DatasetModel } from "../../models/dataset.model";
import { ChatQueryService } from '../../services/chat-query.service';
import { ChatQueryResponseModel } from "../../models/chat-query-response.model";
import { ChartjsModule } from "@ctrl/ngx-chartjs";
import { DatasetsService } from "../../services/datasets.service";

@Component({
    selector: 'app-data-window',
    imports: [
        VisualizedDataComponent,
        TextOutputComponent,
        ChartjsModule
    ],
    templateUrl: './data-window.component.html',
    styleUrl: './data-window.component.scss'
})
export class DataWindowComponent implements OnInit {
    dataset: DatasetModel | undefined;
    promptData: ChatQueryResponseModel | undefined;
    type = '';

    constructor(private chatQueryService: ChatQueryService, private datasetsService: DatasetsService) {
    }

    ngOnInit() {
        this.datasetsService.selectedDataset$.subscribe((dataset) => {
            this.dataset = dataset;
        });
        this.chatQueryService.promptData$.subscribe((promptData) => {
            this.promptData = promptData;
            this.type = promptData.type;
        });
    }
}
