import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

export let latency = new Trend('latency_ms');

export let options = {
  stages: [
    { duration: '30s', target: 50 },   // monter à 50 VUs
    { duration: '1m',  target: 200 },  // tenir 200 VUs
    { duration: '30s', target: 0 },    // redescendre
  ],
  thresholds: {
    'http_req_duration': ['p(95)<1000'], // objectif 95 % < 1 s
  },
};

export default function () {
  // Choisis un productId existant (remplace par un vrai GUID ou 
  // prioritairement : pré-crée des produits et récupère leurs IDs).
  const productId = '3fa85f64-5717-4562-b3fc-2c963f66afa6';

  const payload = JSON.stringify({
    productId: productId,
    quantity:  Math.floor(Math.random() * 10) + 1
  });

  const params = {
    headers: { 'Content-Type': 'application/json' },
  };

  let res = http.post(
    `${__ENV.BASE_URL}/orders`,
    payload,
    params
  );

  // on vérifie qu'on obtient un 201 Created
  check(res, { 'status is 201': (r) => r.status === 201 });

  latency.add(res.timings.duration);
  sleep(0.2);
}
