import React from 'react'
import PropTypes from 'prop-types'
import { FullNameField } from '../../_design'

const UserTitle = ({ record }) => (record ? <FullNameField record={record} size="32" /> : null)

UserTitle.propTypes = {
    record: PropTypes.object,
}

export default UserTitle
