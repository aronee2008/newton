import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { NgbPagination } from '@ng-bootstrap/ng-bootstrap';
import { GameService } from '../game-service';

@Component({
  imports: [RouterLink, NgbPagination, DatePipe, CurrencyPipe],
  selector: 'app-game-list',
  templateUrl: './game-list.html',
})
export class GameList {
  private readonly gameService = inject(GameService);

  readonly games = toSignal(this.gameService.getAll(), { initialValue: [] });
  readonly page = signal(1);
  readonly pageSize = 10;

  readonly pageOfGames = computed(() => {
    const start = (this.page() - 1) * this.pageSize;
    return this.games().slice(start, start + this.pageSize);
  });
}
