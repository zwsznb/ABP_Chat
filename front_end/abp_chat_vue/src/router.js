let routes = [
    {
        path: '/login',
        name: 'Login',
        component: () => import('@/components/login.vue'),
        meta: {
            keepAlive: false
        }
    },
    {
        path: '/',
        redirect: '/login',
        meta: {
            keepAlive: false
        }
    },
    {
        path: '/home',
        name: 'Home',
        component: () => import('@/components/home.vue'),
        meta: {
            keepAlive: true
        }
    },
]

export default routes;