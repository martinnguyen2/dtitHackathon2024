import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { environment } from '../../enviroments/environment';
import { DatasetModel } from '../models/dataset.model';

@Injectable({
  providedIn: 'root'
})
export class DatasetsService {
  private selectedDataset = new ReplaySubject<DatasetModel>(1);
  selectedDataset$ = this.selectedDataset.asObservable();
  private predictiveSet: number = 0; 

  constructor(private http: HttpClient) { }

  public getDatasets():Observable<DatasetModel[]> {
    return this.http.get<DatasetModel[]>(environment.baseUrl + '/api/Datasets');
  }

  public postDataset(input : FormData):Observable<{message:string}>{
    return this.http.post<{message:string}>(environment.baseUrl + "/api/Datasets", input)
  }

  setDataset(dataset: DatasetModel) {
    this.selectedDataset.next(dataset);
  }

  setPredictiveSet(input: number){
    this.predictiveSet = input;
  }
  getPredictiveSet(){
    return this.predictiveSet;
  }
}
