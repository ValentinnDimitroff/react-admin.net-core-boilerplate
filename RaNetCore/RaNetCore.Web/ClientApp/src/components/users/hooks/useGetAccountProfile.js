import { useState, useEffect } from 'react'
import { httpClient } from '../../../ra-providers/dataProvider'
import { apiCustomRoutes } from '../../../constants/api.constants'

const useGetAccountProfile = () => {
    const [state, setState] = useState({
        loading: true,
        loaded: false,
    })

    useEffect(() => {
        httpClient(apiCustomRoutes.accounts.Profile)
            .then((data) => {
                setState({
                    loading: false,
                    loaded: true,
                    data: data.json,
                })
            })
            .catch((error) => {
                setState({
                    loading: false,
                    loaded: true,
                    error: error,
                })
            })
    }, [setState])

    return state
}

export default useGetAccountProfile
