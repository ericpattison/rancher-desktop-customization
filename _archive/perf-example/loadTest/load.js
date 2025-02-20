import http from 'k6/http';

export const options = {
    stages: [
        {duration: '30s', target:20},
        {duration: '1m30s', target: 10},
        {duration: '20s', target:0},
    ],
};

export default function() {
    http.get('http://api.lvh.me/svc1/orders/10100');
}