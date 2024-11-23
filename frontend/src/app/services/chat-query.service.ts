import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../enviroments/environment.js';
import { ChatQueryModel } from '../models/chat-query.model';
import { GraphData } from '../models/graph-data.model.js';

@Injectable({
  providedIn: 'root'
})
export class ChatQueryService {

  constructor(private http: HttpClient) {
  }

  postQuery(prompt: string, datasetName: string) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: 'text/plain',
    });

    const body = {
      prompt: prompt,
      dataset: {
        name: datasetName,
      },
    };
    return this.http.post(environment.baseUrl + "/api/ChatPrompt/submit", body, {headers, responseType: 'text'});
  }

  getGraph(graphName : string): Observable<GraphData[]>{
    return this.http.get<GraphData[]>(environment.baseUrl + "/api/Graph?Name=" + graphName);
  }
}
