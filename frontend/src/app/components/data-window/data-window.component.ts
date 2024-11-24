import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { VisualizedDataComponent } from "./visualized-data/visualized-data.component";
import { TextOutputComponent } from "./text-output/text-output.component";
import { DatasetModel } from "../../models/dataset.model";
import { ChatQueryService } from '../../services/chat-query.service';
import { ChatQueryResponseModel } from "../../models/chat-query-response.model";
import { DatasetsService } from "../../services/datasets.service";
import { SpinnerComponent } from "../spinner/spinner.component";
import { ToolBarComponent } from '../tool-bar/tool-bar.component.js';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-data-window',
    imports: [
        VisualizedDataComponent,
        TextOutputComponent,
        SpinnerComponent,
    ],
    templateUrl: './data-window.component.html',
    styleUrl: './data-window.component.scss',
})
export class DataWindowComponent implements OnInit {
    isLoading = false;
    dataset: DatasetModel | undefined;
    promptData: ChatQueryResponseModel | undefined;
    type = '';
    selectedTypes: string[] = [];

    constructor(private chatQueryService: ChatQueryService, private datasetsService: DatasetsService,private ref : ChangeDetectorRef) {
    }

    ngOnInit() {
        this.chatQueryService.isLoading$.subscribe((isLoading) => {
            this.isLoading = isLoading;
        });
        this.datasetsService.selectedDataset$.subscribe((dataset) => { 
            this.dataset = dataset;
            document.querySelector("#selected-dataset")!.innerHTML = `<strong>Selected dataset:</strong> ${dataset?.name}`;
        });
        this.chatQueryService.promptData$.subscribe((promptData) => {
            this.isLoading = false;
            this.promptData = promptData;
            this.type = promptData.type;
            this.selectedTypes.push(promptData.type);
        });
    }

    isBothSelected():boolean {
        return this.selectedTypes.includes('explain') && this.selectedTypes.includes('visualize') && this.selectedTypes.includes('analyze');
    }
}
