import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { GameService } from './game-service';
import { UpdateGameRequest } from './game.model';

describe('GameService', () => {
  let service: GameService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting()] });
    service = TestBed.inject(GameService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('getAll requests the games collection', () => {
    service.getAll().subscribe();

    http.expectOne({ method: 'GET', url: '/api/games' }).flush([]);
  });

  it('getById requests a single game', () => {
    service.getById(7).subscribe();

    http.expectOne({ method: 'GET', url: '/api/games/7' }).flush({});
  });

  it('update sends the game as the PUT body', () => {
    const request: UpdateGameRequest = {
      title: 'Hades',
      developer: 'Supergiant Games',
      platform: 'Pc',
      genre: 'Action',
      releaseDate: '2020-09-17',
      rating: 93,
      price: 24.99,
    };

    service.update(1, request).subscribe();

    const put = http.expectOne({ method: 'PUT', url: '/api/games/1' });
    expect(put.request.body).toEqual(request);
    put.flush(null, { status: 204, statusText: 'No Content' });
  });
});
