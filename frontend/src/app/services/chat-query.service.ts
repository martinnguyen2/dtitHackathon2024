import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../enviroments/environment.js';

@Injectable({
  providedIn: 'root'
})
export class ChatQueryService {

  constructor(private http: HttpClient) { }

  public postQuery(input : string) : Observable<string>{
    return this.http.post<string>(environment.baseUrl + "/api/ChatPrompt/submit",{"prompt" : input});
  }
}
