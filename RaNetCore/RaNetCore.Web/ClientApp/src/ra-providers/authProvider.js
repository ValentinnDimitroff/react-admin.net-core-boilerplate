import decodeJwt from 'jwt-decode'
import {
    LoginPath,
    SignUpPath,
    ForgotPasswordPath,
    ResetPasswordPath,
} from '../constants/api.constants'
import { userRolesChoices } from '../constants/user-roles.constants'

const userlocalStorageKey = 'user'
const userPermissionsLocalStorageKey = 'permissions'
const LOGIN_PATH = '/login'

function SignUpException(title, errors) {
    this.title = title
    this.errors = errors
}

const signUpHandler = (signUpForm) => {
    const request = new Request(SignUpPath, {
        method: 'POST',
        body: JSON.stringify(signUpForm),
        headers: new Headers({ 'Content-Type': 'application/json' }),
    })

    return fetch(request)
        .then((response) => {
            return response.json()
        })
        .then((json) => {
            if (json.status < 200 || json.status >= 300) {
                throw new SignUpException(json.title, json.errors)
            }

            return json
        })
}

const loginHandler = ({ email, password }) => {
    const request = new Request(LoginPath, {
        method: 'POST',
        body: JSON.stringify({ email, password }),
        headers: new Headers({ 'Content-Type': 'application/json' }),
    })

    return fetch(request)
        .then((response) => {
            if (response.status < 200 || response.status >= 300) {
                throw new Error(response.statusText)
            }

            return response.json()
        })
        .then((user) => {
            localStorage.setItem(userlocalStorageKey, JSON.stringify(user))
            const decodedToken = decodeJwt(user.token)

            const permissionsArr = decodedToken.permissions.split(',')

            const permissionsObj = {}

            userRolesChoices.forEach((role) => {
                // insert all roles and assign boolean result
                permissionsObj[`${role.id}`] = permissionsArr.includes(role.id)
            })

            localStorage.setItem(userPermissionsLocalStorageKey, JSON.stringify(permissionsObj))
        })
}

const logoutHandler = () => {
    localStorage.removeItem(userlocalStorageKey)
    localStorage.removeItem(userPermissionsLocalStorageKey)

    return Promise.resolve()
}

const checkErrorHandler = (error) => {
    const status = error.status

    if (status === 401) {
        localStorage.removeItem(userlocalStorageKey)
        return Promise.reject() // results in redirect
    } else if (status === 403) {
        // window.location.replace("/");
        return Promise.resolve()
    }

    return Promise.resolve()
}

const forgotPasswordHandler = (email) => {
    const request = new Request(ForgotPasswordPath, {
        method: 'POST',
        body: JSON.stringify({ email }),
        headers: new Headers({ 'Content-Type': 'application/json' }),
    })

    return fetch(request)
        .then((response) => {
            return response
        })
        .then((json) => {
            if (json.status < 200 || json.status >= 300) {
                throw new Error(json.statusText)
            }

            return json
        })
}

const resetPasswordHandler = (values) => {
    const request = new Request(ResetPasswordPath, {
        method: 'POST',
        body: JSON.stringify({ ...values }),
        headers: new Headers({ 'Content-Type': 'application/json' }),
    })

    return fetch(request)
        .then((response) => {
            return response
        })
        .then((json) => {
            if (json.status < 200 || json.status >= 300) {
                throw new Error(json.statusText)
            }

            return json
        })
}

const checkAuthHandler = () => {
    return localStorage.getItem(userlocalStorageKey)
        ? Promise.resolve()
        : Promise.reject({ redirectTo: LOGIN_PATH })
}

const getPermissionsHandler = () => {
    const permissions = localStorage.getItem(userPermissionsLocalStorageKey)
    return permissions ? Promise.resolve(JSON.parse(permissions)) : Promise.reject()
}

const getCurrentUser = () => {
    const user = JSON.parse(localStorage.getItem(userlocalStorageKey))
    const decodedToken = user && decodeJwt(user.token)

    return {
        ...user,
        ...decodedToken,
    }
}

export default {
    login: loginHandler,
    logout: logoutHandler,
    checkAuth: checkAuthHandler,
    checkError: checkErrorHandler,
    getPermissions: getPermissionsHandler,
    signUp: signUpHandler,
    getCurrentUser: getCurrentUser,
    forgotPassword: forgotPasswordHandler,
    resetPassword: resetPasswordHandler,
}
