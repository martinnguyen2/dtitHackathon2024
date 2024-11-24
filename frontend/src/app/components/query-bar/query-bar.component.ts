import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { ChatQueryService } from '../../services/chat-query.service';
import { FormsModule } from '@angular/forms';
import { DatasetsService } from '../../services/datasets.service';
import { NgClass } from '@angular/common';
import { DatasetModel } from '../../models/dataset.model';
import { ChatQueryModel } from '../../models/chat-query.model';
import { ChatQueryResponseModel } from '../../models/chat-query-response.model';
import { VoiceRecognitionService } from '../../services/voice-recognition.service';
import { Subscription } from 'rxjs';

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
  cacheId = '';
  isSimplifiedAnswer = false;
  micIcon: string = 'mic';
  textRecognitionSubscribe$: Subscription = new Subscription();

  recognizingVoice: boolean = false;

  promptData: ChatQueryResponseModel | undefined;

  constructor(private chatQuery: ChatQueryService, private datasetsService: DatasetsService, private voiceRecognitionService: VoiceRecognitionService) {
  }

  ngOnInit() {
    this.datasetsService.selectedDataset$.subscribe((dataset) => {
      this.selectedDataset = dataset;
      this.isDatasetSelected = !!dataset;
    });
    this.voiceRecognitionService.init();
  }

  resizeTextArea() {
    this.elRef!.nativeElement.style.height = '0';
    this.elRef!.nativeElement.style.height = this.elRef!.nativeElement.scrollHeight + 'px';
  }

  toggleMic() {
    this.micIcon = this.micIcon === 'mic' ? 'mic_off' : 'mic';
    if (this.micIcon === 'mic') {
      this.stopService();
    } else {
      this.startService();
    }
  }

  sendQuery() {
    this.chatQuery.setIsLoading(true);
    const chatQuery: ChatQueryModel = {
      prompt: this.textAreaInput,
      dataset: this.selectedDataset!,
      cacheId: this.cacheId,
      predictorModel : this.datasetsService.getPredictiveSet(),
      isExpert: !this.isSimplifiedAnswer
    }
    this.chatQuery.postQuery(chatQuery).subscribe((response) => {
      this.chatQuery.setIsLoading(false);
      this.chatQuery.prompt = this.textAreaInput;
      this.cacheId = response.cacheId;
      this.promptData = response;
      this.chatQuery.setPromptData(response);
      this.textAreaInput = '';
    });
  }

  public startService() {
    this.recognizingVoice = this.voiceRecognitionService.start() === true ? true : false;

    if (!this.textAreaInput) {
        this.voiceRecognitionService.init();
    }

    this.textRecognitionSubscribe$ = this.voiceRecognitionService.textChanged.subscribe((text) => {
        this.textAreaInput = text;
    });
  }

  public stopService() {
      this.recognizingVoice = this.voiceRecognitionService.stop() === false ? false : true;

      if (this.textRecognitionSubscribe$) {
          this.textRecognitionSubscribe$.unsubscribe();
      }
  }
}
