import { Routes } from '@angular/router';
import { GameEdit } from './games/game-edit/game-edit';
import { GameList } from './games/game-list/game-list';

export const routes: Routes = [
  { path: '', redirectTo: 'games', pathMatch: 'full' },
  { path: 'games', component: GameList },
  { path: 'games/:id/edit', component: GameEdit },
];
