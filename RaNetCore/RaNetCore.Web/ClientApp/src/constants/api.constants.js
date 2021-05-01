const API_PREFIX = process.env.REACT_APP_API_PREFIX

export const LoginPath = `${API_PREFIX}/auth/login`
export const SignUpPath = `${API_PREFIX}/auth/signup`
export const ForgotPasswordPath = `${API_PREFIX}/auth/forgotPassword`
export const ResetPasswordPath = `${API_PREFIX}/auth/resetPassword`

export const apiCustomRoutes = {
    accounts: {
        Profile: `${API_PREFIX}/Accounts/Profile`,
        UploadPicture: `${API_PREFIX}/Accounts/UploadPicture`,
    },
}