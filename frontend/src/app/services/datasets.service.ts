import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../enviroments/environment';

@Injectable({
  providedIn: 'root'
})
export class DatasetsService {

  constructor(private http: HttpClient) { }

  public postDataset(input : FormData):Observable<string>{
    return this.http.post<string>(environment.baseUrl + "/api/Datasets", input)
  }

  public getDataSet(){

  }
}
