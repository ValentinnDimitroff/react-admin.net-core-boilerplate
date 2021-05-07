import React from 'react'
import PropTypes from 'prop-types'
import { makeStyles } from '@material-ui/styles'
import Divider from '@material-ui/core/Divider'
import CardContent from '@material-ui/core/CardContent'
import CardActions from '@material-ui/core/CardActions'
import {
    FormWithRedirect,
    SaveButton,
} from 'react-admin'

const useStyles = makeStyles(() => ({
    actions: {
        justifyContent: 'flex-end',
    },
}))

const AccountSectionForm = ({ children, onSubmit, submitBtnText = 'Save', ...props }) => {
    const classes = useStyles()

    return (
        <FormWithRedirect
            {...props}
            redirect={false}
            save={onSubmit}
            onSubmit={onSubmit}
            //validateOnBlur
            variant="outlined"
            render={(formProps) => {
                return (
                    <form>
                        {/* autoComplete="off" */}
                        <CardContent>
                            {children(formProps)}
                        </CardContent>
                        <Divider />
                        <CardActions className={classes.actions}>
                            <SaveButton
                                // {...formProps}
                                label={submitBtnText}
                                saving={formProps.saving}
                                handleSubmitWithRedirect={formProps.handleSubmitWithRedirect}
                            />
                        </CardActions>
                    </form>
                )
            }}
        />
    )
}

AccountSectionForm.propTypes = {

}

export default AccountSectionForm
