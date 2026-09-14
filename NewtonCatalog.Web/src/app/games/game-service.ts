import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Game, UpdateGameRequest } from './game.model';

@Injectable({ providedIn: 'root' })
export class GameService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/games';

  getAll() {
    return this.http.get<Game[]>(this.baseUrl);
  }

  getById(id: number) {
    return this.http.get<Game>(`${this.baseUrl}/${id}`);
  }

  update(id: number, game: UpdateGameRequest) {
    return this.http.put<void>(`${this.baseUrl}/${id}`, game);
  }
}
