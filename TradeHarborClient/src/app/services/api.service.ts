import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TradePost } from '../models/tradePost';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl: string = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  public getTradePosts(): Observable<TradePost[]> {
    return this.http.get<TradePost[]>(this.apiUrl + 'trades/gettrades')
  }
}
