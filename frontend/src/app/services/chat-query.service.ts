import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject } from 'rxjs';
import { environment } from '../../enviroments/environment.js';
import { ChatQueryModel } from '../models/chat-query.model';
import { ChatQueryResponseModel } from '../models/chat-query-response.model';
import { GraphData } from '../models/graph-data.model.js';

@Injectable({
  providedIn: 'root'
})
export class ChatQueryService {
  private promptData = new ReplaySubject<ChatQueryResponseModel>(1)
  promptData$ = this.promptData.asObservable();
  constructor(private http: HttpClient) {
  }

  postQuery(query: ChatQueryModel): Observable<ChatQueryResponseModel> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: 'text/plain',
    });

    return this.http.post<ChatQueryResponseModel>(environment.baseUrl + "/api/ChatPrompt/submit", query, { headers });
  }

  getGraph(graphName : string): Observable<GraphData[]>{
    return this.http.get<GraphData[]>(environment.baseUrl + "/api/Graph?Name=" + graphName);
  }

  setPromptData(promptData: ChatQueryResponseModel) {
    this.promptData.next(promptData);
  }
}
