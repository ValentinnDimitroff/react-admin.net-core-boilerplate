import simpleRestProvider from 'ra-data-simple-rest'
import { cacheDataProviderProxy, fetchUtils } from 'react-admin'
import handleImagesAndRespond from './dataProvider-utils/images-handler'

const API_PREFIX = process.env.REACT_APP_API_PREFIX

const CLIENT_CACHE_PERIOD_IN_SECONDS = 30

const httpClient = (url, options = {}) => {
    if (!options.headers) {
        options.headers = new Headers({ Accept: 'application/json' })
    }

    // add your own headers here
    const user = JSON.parse(localStorage.getItem('user'))

    if (user && user.token) {
        options.headers.set('Authorization', `Bearer ${user.token}`)
    }

    return fetchUtils.fetchJson(url, options)
}

const httpBaseCall = (url, httpOptions) => {
    const { onSuccess, onFailure, ...restOptions } = httpOptions

    return httpClient(url, restOptions)
        .then((response) => {
            // console.log('response', response);
            return response.json
        })
        .then((data) => {
            // console.log('data', data);
            onSuccess && onSuccess(data)
            return data
        })
        .catch((error) => {
            onFailure && onFailure(error)
        })
}

const httpGet = (url, options = {}) => {
    return httpBaseCall(url, options)
}

const httpPost = (url, values, options = {}) => {
    return httpBaseCall(url, {
        ...options,
        method: 'POST',
        body: JSON.stringify(values),
    })
}

const httpDelete = (url, id, options = {}) => {
    return httpBaseCall(`${url}/${id}`, {
        ...options,
        method: 'DELETE',
    })
}

const restDataProvider = simpleRestProvider(API_PREFIX, httpClient)

const myDataProvider = {
    ...restDataProvider,
    create: (resource, params) => {
        if (!params.data.images) {
            // fallback to the default implementation
            return restDataProvider.create(resource, params)
        }

        return handleImagesAndRespond('create', restDataProvider, resource, params)
    },
    update: (resource, params) => {
        if (!params.data.images) {
            // fallback to the default implementation
            return restDataProvider.update(resource, params)
        }

        return handleImagesAndRespond('update', restDataProvider, resource, params)
    },
}

export { httpClient, httpPost, httpDelete, httpGet }

export default cacheDataProviderProxy(myDataProvider, CLIENT_CACHE_PERIOD_IN_SECONDS)