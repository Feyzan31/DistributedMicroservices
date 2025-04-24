// no_sql_test.js
import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

// On mesure la latence (en ms)
export let latency = new Trend('latency_ms');

export let options = {
  stages: [
    // Phase de montée en charge : de 0 à 100 VUs en 30s
    { duration: '30s', target: 100 },
    // Charge soutenue à 200 VUs pendant 1min
    { duration: '1m',  target: 200 },
    // Rétrogradation progressive de 200 à 0 VUs en 30s
    { duration: '30s', target: 0 },
  ],
  thresholds: {
    // Objectif : 95 % des requêtes < 500 ms
    'http_req_duration': ['p(95)<500'],
  },
};

export default function () {
  // On interroge l'API, l'URL est injectée via la variable d'environnement BASE_URL
  let res = http.get(`${__ENV.BASE_URL}/products`);
  // Vérifie qu'on obtient bien un 200
  check(res, { 'status is 200': (r) => r.status === 200 });
  // Enregistre la latence de cette requête
  latency.add(res.timings.duration);
  // Pause très courte pour augmenter le débit : ~5 requêtes/s par VU
  sleep(0.2);
}
