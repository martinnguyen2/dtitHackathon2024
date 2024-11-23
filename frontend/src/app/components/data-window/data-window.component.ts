import { Component, OnInit } from '@angular/core';
import { VisualizedDataComponent } from "./visualized-data/visualized-data.component";
import { TextOutputComponent } from "./text-output/text-output.component";
import { DatasetModel } from "../../models/dataset.model";
import { DatasetsService } from "../../services/datasets.service";
import { ChatQueryService } from '../../services/chat-query.service';
import { GraphData } from "../../models/graph-data.model";
import { switchMap } from "rxjs";

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
    graphData: GraphData[] = [];

    constructor(private datasetsService: DatasetsService, private chatQueryService: ChatQueryService) {
    }

    ngOnInit() {
        this.datasetsService.selectedDataset$
            .pipe(
                switchMap((dataset) => {
                    this.dataset = dataset;
                    return this.chatQueryService.getGraph(this.dataset.name);
                })
            )
            .subscribe((graphData) => {
                this.graphData = graphData;
            });
    }
}
