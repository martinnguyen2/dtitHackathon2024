import { Component } from '@angular/core';
import { DatasetsService } from '../../services/datasets.service';

@Component({
  selector: 'app-tool-bar',
  imports: [],
  templateUrl: './tool-bar.component.html',
  styleUrl: './tool-bar.component.scss'
})
export class ToolBarComponent {

  constructor(private datasetService: DatasetsService){}

  public onFileSelected(event : any){

    const file:File = event.target.files[0];
    if(!file)
      return;

    const dataToSend = new FormData()

    dataToSend.append("file",file)

    this.datasetService.postDataset(dataToSend).subscribe(
      next => console.log(next)
    );
  }
}
