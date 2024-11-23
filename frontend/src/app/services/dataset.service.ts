import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {DatasetModel} from '../models/dataset.model';

@Injectable({
  providedIn: 'root'
})
export class DatasetService {

  constructor(private http: HttpClient) { }

  getDatasets() {
    return this.http.get<DatasetModel[]>('http://localhost:5290/api/Datasets');
  }
}
