import { Component, ElementRef, HostListener, ViewChild, viewChild } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-query-bar',
  imports: [MatIconModule],
  templateUrl: './query-bar.component.html',
  styleUrl: './query-bar.component.scss'
})
export class QueryBarComponent {

  @HostListener(':input')
  onInput() {
    this.resizeTextArea();
  }
  @ViewChild('query') elRef : ElementRef | undefined;

  constructor(){}

  resizeTextArea(){
    this.elRef!.nativeElement.style.height = '0';
    this.elRef!.nativeElement.style.height = this.elRef!.nativeElement.scrollHeight + 'px';
  }
}
