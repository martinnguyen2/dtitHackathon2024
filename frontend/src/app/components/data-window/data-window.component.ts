import { Component } from '@angular/core';
import {VisualizedDataComponent} from "./visualized-data/visualized-data.component";
import {TextOutputComponent} from "./text-output/text-output.component";

@Component({
  selector: 'app-data-window',
  imports: [
    VisualizedDataComponent,
    TextOutputComponent
  ],
  templateUrl: './data-window.component.html',
  styleUrl: './data-window.component.scss'
})
export class DataWindowComponent {

}
