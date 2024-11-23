import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { ChatQueryService } from '../../services/chat-query.service';
import { FormsModule } from '@angular/forms';
import { DatasetsService } from '../../services/datasets.service';
import { NgClass } from '@angular/common';
import { DatasetModel } from '../../models/dataset.model';
import { ChatQueryModel } from '../../models/chat-query.model';

@Component({
  selector: 'app-query-bar',
  imports: [MatIconModule, FormsModule, NgClass],
  templateUrl: './query-bar.component.html',
  styleUrl: './query-bar.component.scss'
})
export class QueryBarComponent implements OnInit {

  @HostListener(':input')
  onInput() {
    this.resizeTextArea();
  }

  @ViewChild('query') elRef: ElementRef | undefined;

  public textAreaInput: string = "";
  selectedDataset: DatasetModel | undefined;
  isDatasetSelected = false;

  constructor(private chatQuery: ChatQueryService, private datasetsService: DatasetsService) {
  }

  ngOnInit() {
    this.datasetsService.selectedDataset$.subscribe((dataset) => {
      this.selectedDataset = dataset;
      this.isDatasetSelected = !!dataset;
    });
  }

  resizeTextArea() {
    this.elRef!.nativeElement.style.height = '0';
    this.elRef!.nativeElement.style.height = this.elRef!.nativeElement.scrollHeight + 'px';
  }

  sendQuery() {
    this.chatQuery.postQuery( this.textAreaInput,  this.selectedDataset?.name!).subscribe();
  }
}
