import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../enviroments/environment';
import { DatasetModel } from '../models/dataset.model';

@Injectable({
  providedIn: 'root'
})
export class DatasetsService {

  constructor(private http: HttpClient) { }

  public getDatasets():Observable<DatasetModel[]> {
    return this.http.get<DatasetModel[]>(environment.baseUrl + '/api/Datasets');
  }

  public postDataset(input : FormData):Observable<string>{
    return this.http.post<string>(environment.baseUrl + "/api/Datasets", input)
  }

  public getDataSet(){

  }
}
