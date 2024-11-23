import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { ChatQueryService } from '../../services/chat-query.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-query-bar',
  imports: [MatIconModule, FormsModule],
  templateUrl: './query-bar.component.html',
  styleUrl: './query-bar.component.scss'
})
export class QueryBarComponent {

  @HostListener(':input')
  onInput() {
    this.resizeTextArea();
  }
  @ViewChild('query') elRef : ElementRef | undefined;

  public textAreaInput : string = ""; 

  constructor(private chatQuery: ChatQueryService){}

  resizeTextArea(){
    this.elRef!.nativeElement.style.height = '0';
    this.elRef!.nativeElement.style.height = this.elRef!.nativeElement.scrollHeight + 'px';
  }

  sendQuery(){
    console.log(this.textAreaInput)
    this.chatQuery.postQuery(this.textAreaInput).subscribe(response=> console.log(response))
  }
}
